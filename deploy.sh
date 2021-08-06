#!/bin/sh
dotnet clean
rsync -v -r  ./ lookaroond:~/lookaroond --delete
ssh lookaroond 'cd lookaroond && pwd && docker-compose build && docker-compose down && docker-compose up -d'