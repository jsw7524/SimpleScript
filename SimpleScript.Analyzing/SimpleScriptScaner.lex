%using SimpleScript.RunTime;

%namespace SimpleScript.Analyzing

%option stack, minimize, parser, verbose, persistbuffer, unicode, compressNext, embedbuffers

%{
public void yyerror(string format, params object[] args) // remember to add override back
{
	System.Console.Error.WriteLine("Error: line {0} - " + format, yyline);
}
%}
// Single and Multiline comments
CommentStart	\/\*
CommentEnd	\*\/
LineComment	"//".*

//Base Definitions
D		[0-9]
AZ		[a-zA-Z]
AZex 		[a-zA-Z0-9_]+
//Literals
Identifier		\$([a-zA-Z]([a-zA-Z0-9_])*)
IntegerLiteral	{D}+
DoubleLiteral	{D}+(\.{D}+)?
BooleanLiteral	(true|false)
StringLiteral	\".*\"

//Spaces and End of Line
WhiteSpace		[ \t]
Eol				(\r\n?|\n)


Dim			Dim
Bool		bool
Int			int
String		string
Double		double
As			As
OpAssign	=
OpAdd		+
OpMinus		"-"
OpMul		"*"
OpDiv		"/"
OpModul		"%"
LeftPar		"("
RigthPar	")"
OpAnd		and
OpOr		or
OpNot		not
OpEqu		"=="
OpNotEqu	"!="
OpLt		"<"
OpGt		">"
OpGtEq		">="
OpLtEq		"<="
Begin		Begin
End			End
Input		Input
Print		Print
For			For
To			To
Next		Next
If			If
Then		Then
Else		Else
Fi			Fi
While		While
Do			Do

//jsw7524
LeftBracket   "["
RightBracket  "]"

// The states into which this FSA can pass.
%x CMMT		// Inside a comment.
%x CMMT2	// Inside a comment.
%%

//
// Start of Rules
//


// Remove whitespaces.
{WhiteSpace}+				{;}

// End of Line (Haven't yet figured it how to do this :-) )
{Eol}+		{ return (int) Tokens.EOL; }

// Remove these lines 
{LineComment}+					{yy_push_state (CMMT2);}//return (int) Tokens.COMMENT; }
<CMMT2>{
{Eol} { yy_pop_state ();}
}

/* Move to a 'comment' state on seeing comments. */
{CommentStart}					{  yy_push_state (CMMT); }//return (int) Tokens.COMMENT; }

// Inside a block comment.
<CMMT>{
	[^*\n]+				{return (int) Tokens.COMMENT; }
	"*"					{return (int) Tokens.COMMENT; }
	{CommentEnd}		{ yy_pop_state(); return (int) Tokens.COMMENT; }
	<<EOF>>					{ ; /* raise an error. */ }
}

{Identifier}				{ yylval.String = yytext.Substring(1);
						   return (int) Tokens.IDENTIFIER; }

{IntegerLiteral}					{ Int64.TryParse (yytext, NumberStyles.Integer, CultureInfo.CurrentCulture, out yylval.Integer);
						   return (int) Tokens.INTEGER_LITERAL; }

{DoubleLiteral}					{ double.TryParse (yytext, NumberStyles.Float, CultureInfo.CurrentCulture, out yylval.Double); 
						   return (int) Tokens.DOUBLE_LITERAL; }

{StringLiteral}					{ if (yytext.Length > 2) { yylval.String = yytext.Substring(1, yytext.Length - 2); }
									else { yylval.String = ""; }
								return (int) Tokens.STRING_LITERAL; }

{BooleanLiteral}					{ bool.TryParse(yytext, out yylval.Bool);
						   return (int) Tokens.BOOL_LITERAL; }

{OpAssign}	{ return (int) Tokens.OP_ASSIGN; }
{OpAdd}		{ return (int) Tokens.OP_ADD; }
{OpMinus}	{ return (int) Tokens.OP_MINUS; }
{OpMul}		{ return (int) Tokens.OP_MUL; }
{OpDiv}		{ return (int) Tokens.OP_DIV; }
{OpModul}	{ return (int) Tokens.OP_MODUL; }
{LeftPar}	{ return (int) Tokens.OP_LEFT_PAR; }
{RigthPar}	{ return (int) Tokens.OP_RIGHT_PAR; }
{OpAnd}		{ return (int) Tokens.OP_AND; }
{OpOr}		{ return (int) Tokens.OP_OR; }
{OpNot}		{ return (int) Tokens.OP_NOT; }
{OpEqu}		{ return (int) Tokens.OP_EQU; }
{OpNotEqu}	{ return (int) Tokens.OP_NOT_EQU; }
{OpLt}		{ return (int) Tokens.OP_LT; }
{OpGt}		{ return (int) Tokens.OP_GT; }
{OpGtEq}	{ return (int) Tokens.OP_GT_EQ; }
{OpLtEq}		{ return (int) Tokens.OP_LT_EQ; }

{Dim}		{ return (int) Tokens.DIM; }	
{Bool}		{ return (int) Tokens.BOOL; }
{Int}		{ return (int) Tokens.INT; }
{String}	{ return (int) Tokens.STRING; }
{Double}	{ return (int) Tokens.DOUBLE	; }
{As}		{ return (int) Tokens.AS; }
{Begin}		{ return (int) Tokens.BEGIN; }
{End}		{ return (int) Tokens.END; }
{Input}		{ return (int) Tokens.INPUT; }
{Print}		{ return (int) Tokens.PRINT; }
{For}		{ return (int) Tokens.FOR; }
{To}		{ return (int) Tokens.TO; }
{Next}		{ return (int) Tokens.NEXT; }
{If}		{ return (int) Tokens.IF; }
{Then}		{ return (int) Tokens.THEN; }
{Else}		{ return (int) Tokens.ELSE; }
{Fi}		{ return (int) Tokens.FI; }
{While}		{ return (int) Tokens.WHILE; }
{Do}		{ return (int) Tokens.DO; }
//jsw7524
{LeftBracket}          { Console.WriteLine("LeftBracket!!");return (int) Tokens.LeftBracket; }
{RightBracket}         { Console.WriteLine("RightBracket!!");return (int) Tokens.RightBracket; }