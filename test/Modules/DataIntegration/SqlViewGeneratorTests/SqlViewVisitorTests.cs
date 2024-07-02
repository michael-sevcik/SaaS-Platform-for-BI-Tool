﻿using BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests;

[TestFixture]
public class SqlViewVisitorTests
{
    [Test]
    public void TestJoinProcessing()    // TODO: Test different join types and join condition operators
    {
        SourceColumn[] sourceColumns = [
                   new("MzdObd_DatumOd", new SimpleType(SimpleType.Types.Date, false)),
            new("MzdObd_DatumDo", new SimpleType(SimpleType.Types.Date, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ];

        ISourceEntity sourceTable1 = new SourceTable("TabMzdList", null, [
            new("ZamestnanecId", new SimpleType(SimpleType.Types.Integer, false)),
            new("OdpracHod", new SimpleType(SimpleType.Types.Decimal, false)),
            new("HodSaz", new SimpleType(SimpleType.Types.Decimal, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ]);

        ISourceEntity sourceTable2 = new SourceTable("TabMzdObd", null, [
            new("MzdObd_DatumOd", new SimpleType(SimpleType.Types.Date, false)),
            new("MzdObd_DatumDo", new SimpleType(SimpleType.Types.Date, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ]);
        Join join = new(
            Join.Type.inner,
            sourceTable1,
            sourceTable2,
            "join1",
            [
                sourceTable1.GetColumnMapping("IdObdobi"),
                sourceTable1.GetColumnMapping("HodSaz"),
                sourceTable1.GetColumnMapping("OdpracHod"),
                sourceTable1.GetColumnMapping("ZamestnanecId"),
                sourceTable2.GetColumnMapping("IdObdobi"),
                sourceTable2.GetColumnMapping("MzdObd_DatumDo"),
                sourceTable2.GetColumnMapping("MzdObd_DatumOd"),
            ],
            new(JoinCondition.Operator.equals, sourceTable2.GetColumnMapping("IdObdobi"), sourceTable2.GetColumnMapping("IdObdobi")));
        join.AssignColumnOwnership();

        SqlViewVisitor visitor = new("[dbo].");
        join.Accept(visitor);
        string actualJoinRepresentation = visitor.GetSqlView();

        Assert.That(actualJoinRepresentation, Is.Not.Empty);
        Assert.That(actualJoinRepresentation, Is.EqualTo("(SELECT TabMzdList.ZamestnanecId, TabMzdList.OdpracHod, TabMzdObd.MzdObd_DatumOd, TabMzdObd.MzdObd_DatumDo FROM (SELECT ZamestnanecId, OdpracHod, IdObdobi FROM [dbo].TabMzdList) TabMzdList INNER JOIN (SELECT MzdObd_DatumOd, MzdObd_DatumDo, IdObdobi FROM [dbo].TabMzdObd) TabMzdObd ON TabMzdList.IdObdobi = TabMzdObd.IdObdobi)"));

    }

    // TODO:

    //[Test]
    //public void TestEntityMappingProcessing()
    //{
    //    // TODO: the view entity must generate the named view
    //    ISourceEntity sourceTable1 = new SourceTable("TabMzdList", ["ZamestnanecId", "OdpracHod", "IdObdobi"]);
    //    ISourceEntity sourceTable2 = new SourceTable("TabMzdObd", ["MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"]);
    //    Join join = new(
    //        Join.Type.Inner,
    //        sourceTable1,
    //        sourceTable2,
    //        "3",
    //        [
    //            sourceTable1.GetColumnMapping("ZamestnanecId"),
    //            sourceTable1.GetColumnMapping("OdpracHod"),
    //            sourceTable2.GetColumnMapping("MzdObd_DatumOd"),
    //            sourceTable2.GetColumnMapping("MzdObd_DatumDo"),
    //        ],
    //        new(JoinCondition.Operator.Equal, sourceTable1.GetColumnMapping("IdObdobi"), sourceTable2.GetColumnMapping("IdObdobi")));

    //    EntityMapping entityMapping = new(
    //        name: "EmployeeHoursWorked",
    //        sourceEntities: [sourceTable1, sourceTable2, join],
    //        join,
    //        new() {
    //            { "PersonalId", sourceTable1.GetColumnMapping("ZamestnanecId")},
    //            { "HoursCount", sourceTable1.GetColumnMapping("OdpracHod")},
    //            { "DateFrom", sourceTable2.GetColumnMapping("MzdObd_DatumOd")},
    //            { "DateTo", sourceTable2.GetColumnMapping("MzdObd_DatumDo")},
    //        });

    //    SqlViewVisitor visitor = new("[dbo].");
    //    entityMapping.Accept(visitor);
    //    string actualJoinRepresentation = visitor.GetSqlView();

    //    Assert.That(actualJoinRepresentation, Is.Not.Empty);
    //    Assert.That(actualJoinRepresentation, Is.EqualTo("CREATE VIEW [dbo].EmployeeHoursWorked AS SELECT ZamestnanecId AS PersonalId, OdpracHod AS HoursCount, MzdObd_DatumOd AS DateFrom, MzdObd_DatumDo AS DateTo FROM (SELECT TabMzdList.ZamestnanecId, TabMzdList.OdpracHod, TabMzdObd.MzdObd_DatumOd, TabMzdObd.MzdObd_DatumDo FROM (SELECT ZamestnanecId, OdpracHod, IdObdobi FROM [dbo].TabMzdList) TabMzdList INNER JOIN (SELECT MzdObd_DatumOd, MzdObd_DatumDo, IdObdobi FROM [dbo].TabMzdObd) TabMzdObd ON TabMzdList.IdObdobi = TabMzdObd.IdObdobi)"));
    //}
}
