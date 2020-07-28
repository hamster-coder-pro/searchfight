# searchfight
Tranzact task

Do not use any external libraries for your solution, but you may use external libraries or tools for building and testing it.
We will evaluate your skills in object-oriented programming and design. We expect the code to be production-quality, and can easily be maintained and evolved, not just a barebones algorithm.
We expect your submission no later than next 04.08.2020, please commit your solution to https://github.com/ and provide us a link to your repo.

Programming Challenge:
 
Searchfight
To determine the popularity of programming languages on the internet we want to you to write an application that queries search engines (Google, Bing, Yandex, etc) and compares how many results they return, for example:
     C:\> searchfight.exe .net java
    .net: Google: 4450000000 MSN Search: 12354420
    java: Google: 966000000 MSN Search: 94381485
    Google winner: .net
    MSN Search winner: java
    Total winner: .net
 
·         The application should be able to receive a variable amount of words
·         The application should support quotation marks to allow searching for terms with spaces (e.g. searchfight.exe “java script”)
·         The application should support at least two search engines
              *     Keep in mind the things that will increase the test results:
                      1. Test cases
                      2. Using interfaces
                      3. Using IOC container
                      4. SOLID orientation (SOLID questions will be at the interview too)
                      5. Not underdevelop, not overdevelop
