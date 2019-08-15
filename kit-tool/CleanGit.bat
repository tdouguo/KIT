@echo off & setlocal enabledelayedexpansion
echo "reverting"
set path="%1"
if "%1"=="" (
    echo "path is empty"
    goto END
) else (
    call git checkout %path%
    call git clean -fd %path%
)
:END