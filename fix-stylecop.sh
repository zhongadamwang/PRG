#!/bin/bash

dotnet format ./src
dotnet format style ./src --verbosity minimal
