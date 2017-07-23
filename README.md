# docs-generator
Tool to convert MD documentation to PDF
## Usage
There are two projects that can be used: DocsGeneratorGUI and DocsGeneratorConsole. DocsGeneratorGUI is a simple WindowsForms application
with all necessary fields. DocsGeneratorConsole is a simple console application that takes all the needed parameters as arguments.

## Design
This application allows users to give their design of contents, cover page, header and footer.

### Content design
Inside the DocsGenerator project is a style.css file. This file is added to the header of a generated ALL.html file with all the data from 
the downloaded git documentation. <br />
While generating the pdf file, this file is copied to the working tmp directory with all the other html 
files that are used in file generation (header, footer and cover .html files, described down below). This way, the same style.css file can
be used in all html files by adding this line to `<head>`: <br/>
`<link rel="stylesheet" type="text/css" href="./style.css" >`

### Header and footer
Using wkhtmltopdf it is possible to add a html header and footer to the pdf. Header and footer can be edited in the header.html and 
footer.html in the DocsGenerator project.<br />
More information about headers and footers can be found [here](https://wkhtmltopdf.org/usage/wkhtmltopdf.txt "wkhtmltopdf documentation").

### Cover page
Inside the DocsGenerator project is a cover.html file. This html page will be used to create the cover of the pdf file. This file is parsed
before it is copied to the tmp directory, giving the user options to set automated fields. This table lists possible strings that will be
parsed.<br />

| String     | Description               |
| ---------- | ------------------------- |
| `_title_`  | Represents document title |
| `_date_`   | Represents current date   |

