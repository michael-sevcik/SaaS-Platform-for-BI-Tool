namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;

public interface IVisitable
{
    void Accept(IVisitor visitor);
}
