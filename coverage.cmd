md coverage

packages\OpenCover.4.5.3723\OpenCover.Console.exe -register:user -target:packages\NUnit.Runners.2.6.4\tools\nunit-console-x86.exe -targetargs:"RegexMapper.Tests\bin\Debug\RegexMapper.Tests.dll /noshadow /xml=coverage\TestResult.xml" -filter:"+[RegexMapper*]* -[RegexMapper.Tests*]*" -output:coverage\opencovertests.xml

packages\ReportGenerator.2.1.6.0\ReportGenerator.exe -reports:"coverage\opencovertests.xml" -targetdir:"coverage"

pause