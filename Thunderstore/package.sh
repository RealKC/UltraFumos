#/usr/bin/env bash

cd $(dirname $0)

dotnet build --configuration Release ../FumoSkull/FumoSkull/FumoSkull.csproj

zip package.zip CHANGELOG.md icon.png manifest.json README.md UltraFumos.dll
