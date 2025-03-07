https://www.blogdoft.com.br/2020/06/11/como-gerar-relatorios-de-code-coverage-para-codigo-c/

dotnet add package coverlet.msbuild

dotnet tool install -g dotnet-reportgenerator-globaltool

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./TestResults/coverage.opencover.xml

dotnet test --collect:"XPlat Code Coverage" --results-directory TestResults

 reportgenerator -reports:**/coverage.opencover.xml -targetdir:coverage_report




 # Segunda opção (Esse carinha não faz a coberta da forma correta )
 https://renatogroffe.medium.com/net-5-cobertura-de-testes-com-coverlet-7cbec2f052d9


