// Import        := [StaticImport|ImportImport]

// ImportImport  := 'import'[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
// import System;
// import System.Runtime;

// StaticImport  := 'import'[Space]'static'[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
// import static System;
// import static System.Runtime;

// Alias   := 'alias'[Space][Identifier][Space]'='[Space][Identifier][Space]('.'[Space][identifier][Space])*';'
// alias runtime = System.Runtime;