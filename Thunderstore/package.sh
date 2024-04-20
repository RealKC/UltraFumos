#/usr/bin/env bash

cd $(dirname $0)

zip package.zip CHANGELOG.md icon.png manifest.json README.md ../FumoSkull/FumoSkull/bin/Debug/netstandard2.0/UltraFumos.dll
