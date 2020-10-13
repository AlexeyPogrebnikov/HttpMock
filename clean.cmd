rem Removing untracked directories
git clean -f -d

rem Removing ignored directories
git clean -d -X -f

rem Removing ignored files
git clean -f -x