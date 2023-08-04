del ConfigFileParser\VersionInfo.cs
copy ConfigFileParser\BlankVersionInfo.txt ConfigFileParser\VersionInfo.cs
dotnet run --project "ReplaceTextWithVariables" ConfigFileParser/VersionInfo.cs -d {BUILD_DATE_TIME} -cmd "git describe --tags --abbrev=0" {CI_COMMIT_VERSION_TAG} -cmd "git describe --long --always --exclude=* --abbrev=8" {CI_COMMIT_SHORT_SHA} -cmd "git branch --show-current" {CI_COMMIT_BRANCH}
dotnet build -v q
dotnet publish ConfigFileParser -v q -c release -o ConfigFileParser\bin\Release\net7.0\portable\publish
dotnet publish ConfigFileParser -r win-x64 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r win-x86 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r win-arm --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r win-arm64 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r osx-x64 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r linux-x64 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r linux-musl-x64 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r linux-arm --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet publish ConfigFileParser -r linux-arm64 --sc true -v q -c release /p:PublishSingleFile=true;PublishTrimmed=true;-verbosity=quiet
dotnet run --project "Publisher" "../ConfigFileParser/" "../ConfigFileParser/ConfigFileParser/bin/Release/export"
pause