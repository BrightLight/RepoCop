# RepoCop
RepoCop is a repository hook framework written in C#. Currently only Subversion is supported. You can easily configure actions to perform when something gets comitted. You can also assign conditions with these actions to implement complex rules on when these actions should run.

Right now, RepoCop supports pre-commit and post-commit hooks. Pre-commit hooks are usually used to check whether a commit is valid (e.g. commit message specified, no temp files are commited, author is allowed to commit in this part of the repository, etc.) while post-commit hooks are usually used to handle the commit afterwards (e.g. start a build script, update statistics, run code analysis, notify about the commit via e-mail, etc).
With RepoCop all this can be done.

The following screenshots show some common examples how pre-commit hooks can be used to fail a commit (TortoiseSvn was used for the commits). Please check the [Examples](https://github.com/BrightLight/RepoCop/wiki/Examples) page on how this is configured)

A commit was denied because the commit message was too short (or none was specified at all):

![commit was denied because of short commit message](http://download-codeplex.sec.s-msft.com/Download?ProjectName=repocop&DownloadId=166782)

A commit was done to a locked part of the repository were no further changes are allowed.
![commit was denied because of locked part of repository](http://download-codeplex.sec.s-msft.com/Download?ProjectName=repocop&DownloadId=166784)
