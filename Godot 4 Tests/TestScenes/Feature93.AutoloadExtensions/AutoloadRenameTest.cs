﻿#define RENAME_TEST

#if RENAME_TEST
namespace Godot;

[AutoloadRename("NamedAutoLoad1", "namedAutoLoad1")]
[AutoloadRename("NamedAutoLoad2", "namedAutoLoad2")]
public static partial class Autoload
{
}
#endif
