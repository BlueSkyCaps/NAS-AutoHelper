#!/bin/bash
APP_NAME="/Users/bluesky/Desktop/NAS-AutoHelper.app"
PUBLISH_OUTPUT_DIRECTORY="/Users/bluesky/Workspace/Projects/No-Front/NAS-AutoHelper/NAS-AutoHelper/bin/Release/net8.0/osx-arm64/publish/."
# PUBLISH_OUTPUT_DIRECTORY should point to the output directory of your dotnet publish command.
# One example is /path/to/your/csproj/bin/Release/netcoreapp3.1/osx-x64/publish/.
# If you want to change output directories, add `--output /my/directory/path` to your `dotnet publish` command.
INFO_PLIST="/path/to/your/Info.plist"
ICON_FILE="/Users/bluesky/Workspace/Projects/No-Front/NAS-AutoHelper/NAS-AutoHelper/_mac_publish/tray.icns"

if [ -d "$APP_NAME" ]
then
    rm -rf "$APP_NAME"
fi

mkdir "$APP_NAME"

mkdir "$APP_NAME/Contents"
mkdir "$APP_NAME/Contents/MacOS"
mkdir "$APP_NAME/Contents/Resources"

cp "$INFO_PLIST" "$APP_NAME/Contents/Info.plist"
cp "$ICON_FILE" "$APP_NAME/Contents/Resources/$ICON_FILE"
cp -a "$PUBLISH_OUTPUT_DIRECTORY" "$APP_NAME/Contents/MacOS"