REM the first argument is returned as the exit code
REM the second argument is returned in standard out
REM the third argument is returned in error our
@echo %2
@echo %3 >>&2
@exit %1