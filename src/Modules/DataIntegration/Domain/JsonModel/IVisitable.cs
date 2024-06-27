namespace BIManagement.Modules.DataIntegration.Domain.JsonModel;

public interface IVisitable
{
    void Accept(IVisitor visitor);
}
