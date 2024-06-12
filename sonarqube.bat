dotnet sonarscanner begin /k:"Synith" /d:sonar.host.url="http://localhost:9000" /d:sonar.cs.opencover.reportsPaths="**\TestResults\*\*.xml" /d:sonar.coverage.exclusions="**/Program.cs,**/BuilderExtension.cs,**/AutofacModule.cs,**/TestHelper.cs,**/*DbContext.cs" /d:sonar.login="sqp_35612450c65cfbe33b0d5cc6efc71e99d90c2214"
dotnet build
dotnet test --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
dotnet sonarscanner end /d:sonar.login="sqp_35612450c65cfbe33b0d5cc6efc71e99d90c2214"
