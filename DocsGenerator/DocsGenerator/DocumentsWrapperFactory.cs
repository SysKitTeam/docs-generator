using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace DocsGenerator
{
    class DocumentsWrapperFactory
    {
        List<string> processedPaths = new List<string>();
        public List<DocumentsWrapper> UnprocessedDocuments;
        private string absoluteRootPath;
        public List<DocumentsWrapper> GenerateDocumentsWrapperListFromPath(string rootDir)
        {
            absoluteRootPath = rootDir;
            processedPaths = new List<string>();
            UnprocessedDocuments = new List<DocumentsWrapper>();
            List<DocumentsWrapper> docsList = new List<DocumentsWrapper>();
            if (File.Exists(rootDir + "TOC.md"))
            {
                docsList = parseTOC(rootDir);
            }
            else
            {
                return docsList;
                
            }
            for (int i = 0; i < docsList.Count; i++)
            {
                DocumentsWrapper currentDoc = docsList[i];
                if (currentDoc.IsDirectory)
                {
                    docsFillRecursive(ref currentDoc);
                }
            }
            return docsList;
        }

        private void docsFillRecursive(ref DocumentsWrapper docsDir)
        {
            // Check for TOC:
            if (File.Exists(docsDir.GitPath + "TOC.md"))
            {
                docsDir.SubDocuments = parseTOC(docsDir.GitPath);
            }
            else
            {
                return;
                //docsDir.SubDocuments = generateAlphabeticalDocsList(docsDir.GitPath);
            }

            // Check for more directories, if found continue recursive
            for (int i = 0; i < docsDir.SubDocuments.Count; i++)
            {
                DocumentsWrapper currentDoc = docsDir.SubDocuments[i];
                if (currentDoc.IsDirectory)
                {
                    docsFillRecursive(ref currentDoc);
                }
            }
        }

        public List<DocumentsWrapper> parseTOC(string dirPath)
        {
            List<DocumentsWrapper> docsList = new List<DocumentsWrapper>();
            // u structureList se dodaje zadnji dokument (ili folder) svake razine do sad, s tim da ako se u nekom trenutku
            // smanji razina, sve razine veće od toga se zaboravljaju. Na ovaj način se izbjegava rekurzija kod pamćenja odnosa
            // roditelj - dijete što se tiče strukture.
            List<DocumentsWrapper> structureList = new List<DocumentsWrapper>();

            using (StreamReader reader = new StreamReader(dirPath + "TOC.md"))
            {
                string line = string.Empty;
                while((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith("#")) continue;

                    DocumentsWrapper doc = new DocumentsWrapper();
                    string title, fileName;
                    getTitleAndFilenameFromString(line, out title, out fileName);
                    doc.Title = title;
                    int hashcount = countHashes(line);
                    if (hashcount > structureList.Count) // new structure level added.
                    {
                        if (hashcount > 1) // if it is 'just another' level:
                        {
                            try
                            {
                                doc = analyzeFileOrFolder(fileName, doc, structureList.Last().GitPath);
                            } catch (Exception exc)
                            {
                                UnprocessedDocuments.Add(doc);
                                continue;
                            }
                            
                            if (processedPaths.Contains(doc.GitPath))
                            {
                                continue;
                            }
                            structureList.Last().SubDocuments.Add(doc);
                            structureList.Add(doc);
                        } else // if it is the first level of structure
                        {
                            try
                            {
                                doc = analyzeFileOrFolder(fileName, doc, dirPath);
                            } catch (Exception exc)
                            {
                                UnprocessedDocuments.Add(doc);
                                continue;
                            }
                            
                            if (processedPaths.Contains(doc.GitPath))
                            {
                                continue;
                            }
                            docsList.Add(doc);
                            structureList.Add(doc);
                        }
                    } else if (hashcount == structureList.Count) // if it is the same level of structure
                    {
                        if (hashcount > 1)
                        {
                            try
                            {
                                doc = analyzeFileOrFolder(fileName, doc, structureList[hashcount - 2].GitPath);
                            } catch (Exception exc)
                            {
                                UnprocessedDocuments.Add(doc);
                                continue;
                            }
                            
                            if (processedPaths.Contains(doc.GitPath))
                            {
                                continue;
                            }
                            structureList[hashcount - 2].SubDocuments.Add(doc);
                            structureList[hashcount - 1] = doc;
                        } else
                        {
                            try
                            {
                                doc = analyzeFileOrFolder(fileName, doc, dirPath);
                            } catch (Exception exc)
                            {
                                UnprocessedDocuments.Add(doc);
                                continue;
                            }
                            
                            if (processedPaths.Contains(doc.GitPath))
                            {
                                continue;
                            }
                            docsList.Add(doc);
                            structureList[hashcount - 1] = doc;
                        }
                    } else // if the structure level has lowered:
                    {
                        structureList.RemoveRange(hashcount, structureList.Count - hashcount);
                        if (hashcount > 1)
                        {
                            try
                            {
                                doc = analyzeFileOrFolder(fileName, doc, structureList[hashcount - 2].GitPath);
                            } catch (Exception exc)
                            {
                                UnprocessedDocuments.Add(doc);
                                continue;
                            }
                            
                            if (processedPaths.Contains(doc.GitPath))
                            {
                                continue;
                            }
                            structureList[hashcount - 2].SubDocuments.Add(doc);
                            structureList[hashcount - 1] = doc;
                        } else
                        {
                            try
                            {
                                doc = analyzeFileOrFolder(fileName, doc, dirPath);
                            } catch (Exception exc)
                            {
                                UnprocessedDocuments.Add(doc);
                                continue;
                            }
                            
                            if (processedPaths.Contains(doc.GitPath))
                            {
                                continue;
                            }
                            docsList.Add(doc);
                            structureList[hashcount - 1] = doc;
                        }
                    }
                }
            }
            return docsList;
        }

        private DocumentsWrapper analyzeFileOrFolder(string fileName, DocumentsWrapper doc, string relativePath)
        {
            if (fileName.EndsWith(".md"))
            {
                if (File.Exists(relativePath + fileName))
                {
                    doc.fileName = fileName;
                    doc.GitPath = relativePath + fileName;
                    doc.IsDirectory = false;
                }
                else
                {
                    int indexOfDotmd = fileName.LastIndexOf('.');
                    doc.fileName = fileName.Remove(indexOfDotmd) + @"\";
                    if (!Directory.Exists(relativePath + doc.fileName)) throw new Exception("Wrong assumption.");
                    doc.GitPath = relativePath + doc.fileName;
                    doc.IsDirectory = true;
                }
            }
            else
            {
                if (Directory.Exists(relativePath + fileName))
                {
                    doc.fileName = fileName;
                    doc.GitPath = relativePath + fileName;
                    doc.IsDirectory = true;
                }
                else
                {
                    throw new Exception("Error finding referenced file or folder.");
                }
            }
            return doc;
        }

        /// <summary>
        /// Reads the whole file to find a line witch gives a title.
        /// </summary>
        /// <param name="filePath"> Path of the file. </param>
        /// <returns> Title if found, null otherwise. </returns>
        private string getTitleFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return string.Empty;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("title:"))
                    {
                        return line.Split(':')[1].Trim();
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Parses given string to get title and file name from it. If no fileName is given in the string,
        /// then it is generated from the given title as a directory name.
        /// </summary>
        /// <param name="input"> String to be parsed. </param>
        /// <param name="title"> Where title will be stored. </param>
        /// <param name="fileName"> Where filename will be stored. </param>
        private void getTitleAndFilenameFromString(string input, out string title, out string fileName)
        {
            if (input.Contains("("))
            {
                string[] parts = input.Split(']');
                title = parts[0].Substring(3).Trim();
                fileName = parts[1].Replace(")", "").Substring(1).Trim();
            } else
            {
                title = input.Substring(countHashes(input) + 1).Trim();
                fileName = title.Trim().ToLower().Replace(' ', '-') + "\\";
                fileName = fileName.Trim();
            }
            
        }

        /// <summary>
        /// Counts the amount of hashes on the begining of the given string
        /// </summary>
        /// <param name="input">String to be counted.</param>
        /// <returns>Number of hashes on the begining of the string.</returns>
        private int countHashes(string input)
        {
            int i = 0;
            while (input[i].Equals('#'))
            {
                i++;
            }
            return i;
        }
    }
}
