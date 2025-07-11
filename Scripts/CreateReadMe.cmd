:: CreateReadMe.cmd
:: Create the ReadMe.md file from Docs/ReadMe.docx.
:: This command is meant to be run from within the Scripts folder.
SET FAR=C:\Files\Dropbox\Develop\Active\FindAndReplace\FindAndReplace\bin\Debug\net6.0\FindAndReplace.exe
SET SOURCE=..\Docs\ReadMe.docx
SET TARGET=..\README.md
SET PATTERN=ReadmePostProcessing.json

PANDOC -t markdown_strict --embed-resources=true "%SOURCE%" -o "%TARGET%"
"%FAR%" /wait "/workingpath:..\Docs" "/files:%TARGET%" "/patternfile:%PATTERN%"
