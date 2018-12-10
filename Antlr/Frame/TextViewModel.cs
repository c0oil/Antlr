using System;
using System.IO;
using System.Windows.Input;
using Antlr.Grammar;
using Antlr.ViewModel;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Antlr.Frame
{
    public class TextViewModel : ViewModelBase
    {
        private ICommand parseCommand;
        public ICommand ParseCommand => GetDelegateCommand<object>(ref parseCommand, OnParse);

        private string inText;
        public string InText
        {
            get { return inText; }
            set
            {
                inText = value;
                OnPropertyChanged(nameof(InText));
            }
        }

        private void OnParse(object obj)
        {
            try
            {
                string text = @"
select *
from t1, t2
where t1.id = t2.id

SELECT p.*
FROM Production.Product AS p
ORDER BY Name ASC;
GO

select *
from zxc as t1
    inner join qwe t2 on t1.id = t2.id
    inner join asd t3 on t3.id = t2.id";  
                StringReader reader = new StringReader(text);  
                // В качестве входного потока символов устанавливаем ...  
                AntlrInputStream input = new AntlrInputStream(reader);  
                // Настраиваем лексер на этот поток  
                tsqlLexer lexer = new tsqlLexer(input);  
                // Создаем поток токенов на основе лексера  
                CommonTokenStream tokens = new CommonTokenStream(lexer);  
                // Создаем парсер  
                tsqlParser parser = new tsqlParser(tokens);  
                // Specify our entry point  
                //tsqlParser.Query_specificationContext    
                tsqlParser.Tsql_fileContext Tsql_fileContext1 = parser.tsql_file();  
                Console.WriteLine("Tsql_fileContext1.ChildCount = " + Tsql_fileContext1.ChildCount.ToString());  
    
                /*                // Walk it and attach our listener  
                                Antlr4.Runtime.Tree.ParseTreeWalker walker = new Antlr4.Runtime.Tree.ParseTreeWalker();  
                                AntlrTsqListener listener = new AntlrTsqListener();  
                                walker.Walk(listener, Tsql_fileContext1);*/  
                AntlrTsqVisitor visitor = new AntlrTsqVisitor();  
                var result = visitor.Visit(Tsql_fileContext1);
                InText = result;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class AntlrTsqVisitor : tsqlBaseVisitor<String>  
      {  
  /*  
          public override string VisitSql_clauses(tsqlParser.Sql_clausesContext ctx)  
          {  
              Console.WriteLine("VisitSql_clauses");  
              return VisitChildren(ctx).ToString();  
          }  
 */  
          public override string VisitSql_clause(tsqlParser.Sql_clauseContext ctx)  
          {  
              Console.WriteLine("VisitSql_clause");  
              try  
              {  
                  return VisitDml_clause(ctx.dml_clause()).ToString();  
              }  
              catch (Exception e)  
              {  
                  return "";  
              }  
          }  
  /*  
          public override string VisitDml_clause(tsqlParser.Dml_clauseContext ctx)  
          {  
              Console.WriteLine("VisitDml_clause");  
              return VisitChildren(ctx).ToString();  
          }  
 */  
          public override string VisitSelect_statement([NotNull] tsqlParser.Select_statementContext ctx)  
          {  
              Console.WriteLine("VisitSelect_statement");  
              return VisitTable_sources(ctx.query_expression().query_specification().table_sources()).ToString();  
          }  
          public override string VisitDelete_statement([NotNull] tsqlParser.Delete_statementContext ctx)  
          {  
              Console.WriteLine("VisitDelete_statement");  
              try  
              {  
                  return VisitTable_sources(ctx.table_sources());  
              }  
              catch (Exception e)  
              {  
                  return "";  
              }  
          }  
          public override string VisitUpdate_statement([NotNull] tsqlParser.Update_statementContext ctx)  
          {  
              Console.WriteLine("VisitUpdate_statement");  
              try  
              {  
                  return VisitTable_sources(ctx.table_sources());  
              }  
              catch (Exception e)  
              {  
                  return "";  
              }  
          }  
          public override string VisitInsert_statement([NotNull] tsqlParser.Insert_statementContext ctx)  
          {  
              Console.WriteLine("VisitInsert_statement");  
              try  
              {  
                  return VisitTable_sources(ctx.insert_statement_value().derived_table().subquery().select_statement().query_expression().query_specification().table_sources());  
              }  
              catch (Exception e)  
              {  
                  return "";  
              }  
          }  
  /*  
          public override string VisitTable_sources([NotNull] tsqlParser.Table_sourcesContext ctx)  
          {  
              Console.WriteLine("VisitTable_sources");  
              return VisitChildren(ctx).ToString();  
          }  
          public override string VisitTable_source([NotNull] tsqlParser.Table_sourceContext ctx)  
          {  
              Console.WriteLine("VisitTable_source");  
              return VisitChildren(ctx).ToString();  
          }  
          public override string VisitTable_source_item_joined([NotNull] tsqlParser.Table_source_item_joinedContext ctx)  
          {  
              Console.WriteLine("VisitTable_source_item_joined");  
              return VisitChildren(ctx).ToString();  
          }  
 */  
          public override string VisitTable_source_item([NotNull] tsqlParser.Table_source_itemContext ctx)  
          {  
              Console.WriteLine("VisitTable_source_item");  
              int ii = 0;  
              //Console.WriteLine(ctx.ToStringTree());  
              Console.WriteLine("ctx.ChildCount " + ctx.ChildCount.ToString());  
              for (ii = 0; ii < ctx.ChildCount; ++ii)  
              {  
                  Console.WriteLine("ii=" + ii.ToString());  
                  Console.WriteLine(ctx.GetChild(ii).GetType().ToString());  
                  Console.WriteLine(ctx.GetChild(ii).GetText());  
                  //if (ctx.GetChild(ii).GetType().ToString() == "tsqlParser+Table_sourcesContext")  
                  //{  
                  //    this.VisitTable_sources(ctx.table_sources());  
                  //}  
              }  
              //Console.WriteLine(ctx.GetChild<tsqlParser.i>().ToString());  
              return ctx.ToString();  
          }  
          public override string VisitJoin_part([NotNull] tsqlParser.Join_partContext ctx)  
          {  
              Console.WriteLine("VisitJoin_part");  
              int ii = 0;  
              //Console.WriteLine(ctx.ToStringTree());  
              Console.WriteLine("ctx.ChildCount " + ctx.ChildCount.ToString());  
              for (ii = 0; ii < ctx.ChildCount; ++ii)  
              {  
                  Console.WriteLine("ii=" + ii.ToString());  
                  Console.WriteLine(ctx.GetChild(ii).GetType().ToString());  
                  Console.WriteLine(ctx.GetChild(ii).GetText());  
                  if (ctx.GetChild(ii).GetType().ToString() == "tsqlParser+Table_sourceContext")  
                  {  
                      this.VisitTable_source(ctx.table_source());  
                  }  
              }  
              //Console.WriteLine(ctx.GetChild<tsqlParser.i>().ToString());  
              return ctx.ToString();  
          }  
      }  
}