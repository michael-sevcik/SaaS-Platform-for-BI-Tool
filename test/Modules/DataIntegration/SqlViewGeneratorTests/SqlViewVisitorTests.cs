using BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating;
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
        Assert.That(actualJoinRepresentation, Is.EqualTo("(SELECT TabMzdList.TabMzdList__IdObdobi,\r\nTabMzdList.TabMzdList__HodSaz,\r\nTabMzdList.TabMzdList__OdpracHod,\r\nTabMzdList.TabMzdList__ZamestnanecId,\r\nTabMzdObd.TabMzdObd__IdObdobi,\r\nTabMzdObd.TabMzdObd__MzdObd_DatumDo,\r\nTabMzdObd.TabMzdObd__MzdObd_DatumOd\r\nFROM (SELECT TabMzdList.ZamestnanecId AS TabMzdList__ZamestnanecId,\r\nTabMzdList.OdpracHod AS TabMzdList__OdpracHod,\r\nTabMzdList.HodSaz AS TabMzdList__HodSaz,\r\nTabMzdList.IdObdobi AS TabMzdList__IdObdobi\r\n FROM [dbo].TabMzdList) TabMzdList\r\nINNER JOIN (SELECT TabMzdObd.MzdObd_DatumOd AS TabMzdObd__MzdObd_DatumOd,\r\nTabMzdObd.MzdObd_DatumDo AS TabMzdObd__MzdObd_DatumDo,\r\nTabMzdObd.IdObdobi AS TabMzdObd__IdObdobi\r\n FROM [dbo].TabMzdObd) TabMzdObd\r\nON TabMzdObd.TabMzdObd__IdObdobi= TabMzdObd.TabMzdObd__IdObdobi)"));

    }

    // TODO:

    [Test]
    public void TestEntityMappingProcessing()
    {
        // TODO: the view entity must generate the named view
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

        EntityMapping entityMapping = new(
            name: "EmployeeHoursWorked2",
            null,
            [sourceTable1, sourceTable2, join],
            join,
            new() {
                { "PersonalId", sourceTable1.GetColumnMapping("ZamestnanecId")},
                { "HoursCount", sourceTable1.GetColumnMapping("OdpracHod")},
                { "DateFrom", sourceTable2.GetColumnMapping("MzdObd_DatumOd")},
                { "DateTo", null},
                { "note", null}
            });
        entityMapping.SourceEntity!.AssignColumnOwnership();

        SqlViewVisitor visitor = new("[dbo].");
        entityMapping.Accept(visitor);
        string actualJoinRepresentation = visitor.GetSqlView();
        Console.WriteLine();

        Assert.That(actualJoinRepresentation, Is.Not.Empty);
        Assert.That(actualJoinRepresentation, Is.EqualTo("CREATE VIEW [dbo].EmployeeHoursWorked2\r\nAS SELECT join1.TabMzdList__ZamestnanecId AS PersonalId, join1.TabMzdList__OdpracHod AS HoursCount, join1.TabMzdObd__MzdObd_DatumOd AS DateFrom, NULL AS DateTo, NULL AS note\r\nFROM (SELECT TabMzdList.TabMzdList__IdObdobi,\r\nTabMzdList.TabMzdList__HodSaz,\r\nTabMzdList.TabMzdList__OdpracHod,\r\nTabMzdList.TabMzdList__ZamestnanecId,\r\nTabMzdObd.TabMzdObd__IdObdobi,\r\nTabMzdObd.TabMzdObd__MzdObd_DatumDo,\r\nTabMzdObd.TabMzdObd__MzdObd_DatumOd\r\nFROM (SELECT TabMzdList.ZamestnanecId AS TabMzdList__ZamestnanecId,\r\nTabMzdList.OdpracHod AS TabMzdList__OdpracHod,\r\nTabMzdList.HodSaz AS TabMzdList__HodSaz,\r\nTabMzdList.IdObdobi AS TabMzdList__IdObdobi\r\n FROM [dbo].TabMzdList) TabMzdList\r\nINNER JOIN (SELECT TabMzdObd.MzdObd_DatumOd AS TabMzdObd__MzdObd_DatumOd,\r\nTabMzdObd.MzdObd_DatumDo AS TabMzdObd__MzdObd_DatumDo,\r\nTabMzdObd.IdObdobi AS TabMzdObd__IdObdobi\r\n FROM [dbo].TabMzdObd) TabMzdObd\r\nON TabMzdObd.TabMzdObd__IdObdobi= TabMzdObd.TabMzdObd__IdObdobi) join1"));
    }
}
