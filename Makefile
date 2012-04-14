PROJ = src/build.proj
FLAGS = /property:OperatingPlatform=Unix
XBUILD = xbuild /tv:4.0

VERSION =
BUILD =
REVISION =
MATURITY =
VERSTR = $(VERSION).$(BUILD).$(REVISION)
ifdef MATURITY
	SEMVER = $(VERSION).$(REVISION)-$(MATURITY)
else
	MATURITY = Release
	SEMVER = $(VERSION).$(REVISION)
endif

VERSIONINFO = src/Shared/VersionInfo.cs

PACK = tar -czf ../crossroads-net.mono-$(SEMVER).tar.gz
PACKDIR = build
PACKFILES = CrossroadsIO.* ../README.md ../AUTHORS ../LICENSE

.PHONY=all release package clean

all:
	$(XBUILD) $(FLAGS) $(PROJ)

release:
ifdef VERSION
	@echo 'using System.Reflection;' > $(VERSIONINFO).tmp
	@echo >> $(VERSIONINFO).tmp
	@echo '[assembly: AssemblyVersion("$(VERSION).0.0")]' >> $(VERSIONINFO).tmp
	@echo '[assembly: AssemblyFileVersion("$(VERSTR)")]' >> $(VERSIONINFO).tmp
	@echo '[assembly: AssemblyInformationalVersion("$(VERSTR) $(MATURITY)")]' >> $(VERSIONINFO).tmp
	@echo '[assembly: AssemblyConfiguration("$(MATURITY)")]' >> $(VERSIONINFO).tmp
	@mv $(VERSIONINFO) $(VERSIONINFO).bak
	@mv $(VERSIONINFO).tmp $(VERSIONINFO)
	$(XBUILD) /target:Build $(FLAGS) /property:Configuration=Release /property:SignAssembly=true $(PROJ)
	@mv $(VERSIONINFO).bak $(VERSIONINFO)
else
	$(error Invalid VERSION==$(VERSION) - specify package version. E.g., `make release VERSION=3.0 BUILD=12345 REVISION=1 MATURITY=Beta`)
endif

package: release
	cd $(PACKDIR);$(PACK) $(PACKFILES);cd -

clean:
	$(XBUILD) /target:Clean $(FLAGS) $(PROJ)
