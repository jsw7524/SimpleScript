%using SimpleScript.RunTime;
%using SimpleScript.RunTime.Statements;

%namespace SimpleScript.Analyzing

%{
	SymbolTable symTable = SymbolTable.GetInstance;
	public StatementList program = new StatementList();
%}

%start program

%union {
    public long Integer;
    public string String;
    public double Double;
	public bool Bool;
	public Expression expr;
	public StatementList statementList;
	public IStatement  statement;
}
// Defining Tokens
%token COMMENT
%token <String>	 IDENTIFIER
%token <Integer> INTEGER_LITERAL
%token <Double>	 DOUBLE_LITERAL
%token <Bool>	 BOOL_LITERAL
%token <String>	 STRING_LITERAL
%token EOL
%token DIM
%token BOOL
%token INT
%token STRING
%token DOUBLE
%token AS

%token BEGIN
%token END

// I/O Statement
%token PRINT
%token INPUT

// For statement
%token FOR
%token TO
%token NEXT

// While Statement
%token WHILE
%token DO

// If condition. 
%token IF	
%token THEN
%token ELSE
%left FI	

%token OP_RIGHT_PAR
%token OP_LEFT_PAR
%left OP_ASSIGN
%left OP_ADD OP_MINUS
%left OP_MUL OP_DIV 
%left OP_MODUL
%left OP_AND
%left OP_OR
%left OP_NOT
%left OP_EQU
%left OP_NOT_EQU
%left OP_LT
%left OP_GT
%left OP_GT_EQ
%left OP_LT_EQ

//jsw7524 
%token LeftBracket
%token RightBracket


// YACC Rules
%%
program			:	BEGIN EOL statementList EOL END {program = $3.statementList;}
				;

statementList	:	/*Empty*/	{if($$.statementList == null)	{$$.statementList = new StatementList();}}

				|	statement	{	if($$.statementList == null)	{$$.statementList = new StatementList();}
									$$.statementList.InsertFront($1.statement);
									
								}
				|	statementList EOL statement	{ $1.statementList.Add($3.statement); $$.statementList = $1.statementList; }
				;
			

statement	:	varDecl		{ $$.statement = $1.statement; }
			|	assignOp	{ $$.statement = $1.statement; }
			|	printOp		{ $$.statement = $1.statement; }
			|	inputOp		{ $$.statement = $1.statement; }
			|	forLoop		{ $$.statement = $1.statement; }
			|	ifCond		{ $$.statement = $1.statement; }
			|	whileLoop	{ $$.statement = $1.statement; }
			;
// Variable Declaration
varDecl		:	DIM IDENTIFIER AS INT		{int yId = symTable.Add($2); symTable.SetType(yId, SimpleScriptTypes.Integer); $$.statement = new VriableDeclStatement(yId);}
			|	DIM IDENTIFIER AS DOUBLE	{int yId = symTable.Add($2); symTable.SetType(yId, SimpleScriptTypes.Double);  $$.statement = new VriableDeclStatement(yId);}
			|	DIM IDENTIFIER AS BOOL		{int yId = symTable.Add($2); symTable.SetType(yId, SimpleScriptTypes.Boolean); $$.statement = new VriableDeclStatement(yId);}
			|	DIM IDENTIFIER AS STRING	{int yId = symTable.Add($2); symTable.SetType(yId, SimpleScriptTypes.String);  $$.statement = new VriableDeclStatement(yId);}
    		|	DIM IDENTIFIER AS INT LeftBracket INTEGER_LITERAL RightBracket		{Console.WriteLine("Array!! size{0}",$6);int yId = symTable.Add($2); symTable.SetType(yId, SimpleScriptTypes.IntegerArray,$6);  $$.statement = new VriableDeclStatement(yId,$6);}
			;

			
assignOp	:	IDENTIFIER OP_ASSIGN Expr		{$$.statement = new AssignmentStatement(symTable.GetID($1), $3.expr);}
            | 	IDENTIFIER  LeftBracket Expr RightBracket OP_ASSIGN Expr		{Console.WriteLine("Array!! OP_ASSIGN");$$.statement = new AssignmentStatement(symTable.GetID($1), $3.expr, $6.expr);}  
			;

//Grammmar for expressions.
//E->E+T | E-T | T 
//T->T*F | T/F | F 
//F->N | (E) | V 


Expr		:	OP_LEFT_PAR Expr OP_RIGHT_PAR		{ $$.expr = $2.expr; }
			|	Literal						{ $$.expr = $1.expr; }
			|	IDENTIFIER					{ $$.expr = new Expression(symTable.Get($1));}
			|	IDENTIFIER LeftBracket Expr RightBracket					{ Console.WriteLine("read Array {0}!! index {1}",$1,$3);$$.expr= new Expression(symTable.Get($1),$3.expr); }
			|	Expr OP_ADD Expr			{ $$.expr = new Expression(Operation.Add,$1.expr,$3.expr); }
			|	Expr OP_MINUS Expr			{ $$.expr = new Expression(Operation.Sub,$1.expr,$3.expr); }
			|	OP_MINUS Expr %prec OP_MUL	{ $$.expr = new Expression(Operation.UnaryMinus,null,$2.expr); }
			|	Expr OP_MUL Expr			{ $$.expr = new Expression(Operation.Mul,$1.expr,$3.expr); }
			|	Expr OP_DIV Expr			{ $$.expr = new Expression(Operation.Div,$1.expr,$3.expr); }
			|	Expr OP_MODUL Expr			{ $$.expr = new Expression(Operation.Modul,$1.expr,$3.expr); }
			|	Expr OP_AND Expr			{ $$.expr = new Expression(Operation.And,$1.expr,$3.expr); }		
			|	Expr OP_OR  Expr			{ $$.expr = new Expression(Operation.Or,$1.expr,$3.expr); }		
			|	Expr OP_NOT Expr			{ $$.expr = new Expression(Operation.Not,$1.expr,$3.expr); }		
			|	Expr OP_EQU Expr			{ $$.expr = new Expression(Operation.Equ,$1.expr,$3.expr); }
			|	Expr OP_NOT_EQU Expr		{ $$.expr = new Expression(Operation.NotEqu,$1.expr,$3.expr); }
			|	Expr OP_LT  Expr			{ $$.expr = new Expression(Operation.Lt,$1.expr,$3.expr); }		
			|	Expr OP_GT  Expr			{ $$.expr = new Expression(Operation.Gt,$1.expr,$3.expr); }		
			|	Expr OP_GT_EQ Expr			{ $$.expr = new Expression(Operation.GtEq,$1.expr,$3.expr); }	
			|	Expr OP_LT_EQ Expr			{ $$.expr = new Expression(Operation.LtEq,$1.expr,$3.expr); }	
			;

Literal		:	STRING_LITERAL	{$$.expr = new Expression($1);}
			|	BOOL_LITERAL	{$$.expr = new Expression($1);}
			|	INTEGER_LITERAL	{$$.expr = new Expression($1);}
			|	DOUBLE_LITERAL	{$$.expr = new Expression($1);}		
			;

printOp		:	PRINT Expr	{$$.statement = new PrintStatement($2.expr);}
			;

inputOp		:	INPUT IDENTIFIER {$$.statement = new InputStatement(symTable.GetID($2));}
			;
			
forLoop		:	FOR OP_LEFT_PAR IDENTIFIER OP_ASSIGN Expr TO Expr OP_RIGHT_PAR EOL forBody NEXT 
				{$$.statement = new ForStatement(symTable.Get($3) as SymbolTableIntegerElement, $5.expr, $7.expr, $10.statementList);}
			;

forBody		:	/*Empty*/			{$$.statementList = new StatementList();}
			|	statementList EOL	{$$.statementList = $1.statementList;}
			;

ifCond		:	IF OP_LEFT_PAR Expr OP_RIGHT_PAR THEN EOL ifBody else FI
				{$$.statement = new IfCondStatement($3.expr,$7.statementList,$8.statementList);}
			;

ifBody		:	/*Empty*/				{$$.statementList = new StatementList();}
			|	statementList EOL		{$$.statementList = $1.statementList;}
			;

else		:	/* Empty */			{$$.statementList = new StatementList();}
			|	ELSE EOL elseBody	{$$.statementList = $3.statementList;}
			;

elseBody	:	/*Empty*/			{$$.statementList = new StatementList();}
			|	statementList EOL	{$$.statementList = $1.statementList;}
			;

whileLoop	:	WHILE OP_LEFT_PAR Expr OP_RIGHT_PAR DO EOL whileBody NEXT
				{$$.statement = new WhileLoopStatement($3.expr,$7.statementList);}
			;

whileBody	:	/*Empty*/			{$$.statementList = new StatementList();}
			|	statementList EOL	{$$.statementList = $1.statementList;}
			;								
%%

// No argument CTOR. By deafult Parser's ctor requires scanner as param.
public Parser(Scanner scn) : base(scn) { }