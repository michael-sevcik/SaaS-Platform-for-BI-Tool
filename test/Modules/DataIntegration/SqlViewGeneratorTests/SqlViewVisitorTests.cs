using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators.Conditions;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.SqlViewGenerating;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests;

[TestFixture]
public class SqlViewVisitorTests
{
    [Test]
    public void TestJoinProcessing()    // TODO: Test different join types and join condition operators
    {
        ISourceEntity sourceTable1 = new SourceTable("TabMzdList", new string[] { "ZamestnanecId", "OdpracHod", "IdObdobi" });
        ISourceEntity sourceTable2 = new SourceTable("TabMzdObd", new string[] { "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi" });
        Join join = new(
            Join.Type.Inner,
            sourceTable1,
            sourceTable2,
            "3",
            new ColumnMapping[] {
                sourceTable1.GetColumnMapping("ZamestnanecId"),
                sourceTable1.GetColumnMapping("OdpracHod"),
                sourceTable2.GetColumnMapping("MzdObd_DatumOd"),
                sourceTable2.GetColumnMapping("MzdObd_DatumDo"),
            },
            new(JoinCondition.Operator.Equal, sourceTable1.GetColumnMapping("IdObdobi"), sourceTable2.GetColumnMapping("IdObdobi")));

        SqlViewVisitor visitor = new SqlViewVisitor("[dbo].");
        join.Accept(visitor);
        string actualJoinRepresentation = visitor.GetSqlView();

        Assert.That(actualJoinRepresentation, Is.Not.Empty);
        Assert.That(actualJoinRepresentation, Is.EqualTo("(SELECT TabMzdList.ZamestnanecId, TabMzdList.OdpracHod, TabMzdObd.MzdObd_DatumOd, TabMzdObd.MzdObd_DatumDo FROM (SELECT ZamestnanecId, OdpracHod, IdObdobi FROM [dbo].TabMzdList) TabMzdList INNER JOIN (SELECT MzdObd_DatumOd, MzdObd_DatumDo, IdObdobi FROM [dbo].TabMzdObd) TabMzdObd ON TabMzdList.IdObdobi = TabMzdObd.IdObdobi)"));

    }

    [Test]
    public void TestEntityMappingProcessing()
    {
        // TODO: the view entity must generate the named view
        ISourceEntity sourceTable1 = new SourceTable("TabMzdList", new string[] { "ZamestnanecId", "OdpracHod", "IdObdobi" });
        ISourceEntity sourceTable2 = new SourceTable("TabMzdObd", new string[] { "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi" });
        Join join = new(
            Join.Type.Inner,
            sourceTable1,
            sourceTable2,
            "3",
            new ColumnMapping[] {
                sourceTable1.GetColumnMapping("ZamestnanecId"),
                sourceTable1.GetColumnMapping("OdpracHod"),
                sourceTable2.GetColumnMapping("MzdObd_DatumOd"),
                sourceTable2.GetColumnMapping("MzdObd_DatumDo"),
            },
            new(JoinCondition.Operator.Equal, sourceTable1.GetColumnMapping("IdObdobi"), sourceTable2.GetColumnMapping("IdObdobi")));

        EntityMapping entityMapping = new(
            name: "EmployeeHoursWorked",
            sourceEntities: new ISourceEntity[] { sourceTable1, sourceTable2, join },
            join,
            new() {
                { "PersonalId", sourceTable1.GetColumnMapping("ZamestnanecId")},
                { "HoursCount", sourceTable1.GetColumnMapping("OdpracHod")},
                { "DateFrom", sourceTable2.GetColumnMapping("MzdObd_DatumOd")},
                { "DateTo", sourceTable2.GetColumnMapping("MzdObd_DatumDo")},
            });

        SqlViewVisitor visitor = new SqlViewVisitor("[dbo].");
        entityMapping.Accept(visitor);
        string actualJoinRepresentation = visitor.GetSqlView();

        Assert.That(actualJoinRepresentation, Is.Not.Empty);
        Assert.That(actualJoinRepresentation, Is.EqualTo("CREATE VIEW [dbo].EmployeeHoursWorked AS SELECT ZamestnanecId AS PersonalId, OdpracHod AS HoursCount, MzdObd_DatumOd AS DateFrom, MzdObd_DatumDo AS DateTo FROM (SELECT TabMzdList.ZamestnanecId, TabMzdList.OdpracHod, TabMzdObd.MzdObd_DatumOd, TabMzdObd.MzdObd_DatumDo FROM (SELECT ZamestnanecId, OdpracHod, IdObdobi FROM [dbo].TabMzdList) TabMzdList INNER JOIN (SELECT MzdObd_DatumOd, MzdObd_DatumDo, IdObdobi FROM [dbo].TabMzdObd) TabMzdObd ON TabMzdList.IdObdobi = TabMzdObd.IdObdobi)"));
    }
}
