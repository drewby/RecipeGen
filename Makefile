all : restore build test

clean: clean-bin
	dotnet clean
	rm -rf out
	rm -rf tst/webapi/TestResults
	rm -rf reports

clean-bin:
	rm -rf src/webapi/bin src/webapi/obj
	rm -rf tst/webapi/bin tst/webapi/obj

restore:
	dotnet restore

build: 
	dotnet build

test: 
	@mkdir -p reports
	@dotnet test --collect:"XPlat Code Coverage" 
	@~/.dotnet/tools/reportgenerator  -reports:tst/webapi/TestResults/**/*.xml -targetdir:reports/dotnet-coverage -reporttype:Html
	@cd src/app && npm install && CI=true npm test -- --coverage
	@rm -rf ./reports/node-coverage
	@mv -f src/app/coverage/lcov-report ./reports/node-coverage

run:
	dotnet run --project src/webapi