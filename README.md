# karna-compression
Zip compression library for .NET

Many of today's applications require the capability of extracting certain files from a ZIP archive, either onto the hard disk or into memory.

Info-ZIP is an Open Source version of Phil Katz's "deflate" and "inflate" routines used in his popular file compression program, PKZIP. Info-ZIP code has been incorporated into a number of third-party products as well, both commercial and freeware. It offers two dynamic link libraries: one for zipping, and one for unzipping.

The Info-ZIP DLLs are free to use and distribute, but they are designed to be used in C/C++ projects, so they're not really .NET-friendly. Also, the Info-ZIP package contains almost no documentation showing how to use the Info-ZIP DLLs.

Therefore, I decided to write a small C# wrapper that provides all the required data types and functions in order to give the possibility to work with the Info-ZIP API.

More information about the Info-ZIP project can be found on Info-ZIP's home site: www.info-zip.org.
