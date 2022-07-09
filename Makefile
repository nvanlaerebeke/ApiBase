.PHONY: build clean app test push

PROJECT=ApiBase
CONFIGURATION:=Release
VERSION:=${shell cat VERSION}
REGISTRY:=https://nuget.crazyzone.be/v3/index.json

APIKEY:=

clean:
	rm -rf sample/*/bin/
	rm -rf sample/*/obj/
	rm -rf src/*/bin/
	rm -rf src/*/obj/
	rm -rf TestResults
	rm -rf build

app: 
	dotnet restore $(PROJECT).sln
	dotnet publish $(PROJECT).sln /t:$(PROJECT) /p:Configuration="$(CONFIGURATION)" /p:Platform="Any CPU" /p:Version=$(VERSION)

build: clean app

push: build
	dotnet nuget push src/${PROJECT}/bin/Release/*.nupkg -s "$(REGISTRY)" -k "$(APIKEY)"

test: clean
	dotnet restore $(PROJECT).sln
	dotnet build $(PROJECT).sln
	dotnet test "$(PROJECT).sln" --logger "trx" --results-directory "`pwd`/TestResults" --no-build
