# TranslationManagement_API
API for managing available translators, jobs to translate etc.
#To run backend locally, do the following

Open Bash, cd to the folder with the solution
Run 'docker build --rm -t rws/translation-api:latest .' command to build the app.
Run 'docker run --rm -p 5184:5184 -e ASPNETCORE_URLS=http://+:5184 rws/translation-api' to run the app in a docker container
