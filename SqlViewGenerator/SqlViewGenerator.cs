using SqlViewGenerator.JsonModel;
using SqlViewGenerator.SqlViewGenerating;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlViewGenerator;

public static class SqlViewGenerator
{
    public static string GenerateView(EntityMapping mapping)
    {
        SqlViewVisitor visitor = new ();
        mapping.Accept(visitor);
        return visitor.GetSqlView();
    }
}
