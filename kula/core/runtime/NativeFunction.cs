namespace Kula.Core.Runtime;


public class NativeFunction : ICallable {
    public delegate object? Callee(object? _this, List<object?> arguments);

    private int arity;
    private Callee callee;
    private object? @this;

    int ICallable.Arity => arity;

    object? ICallable.Call(List<object?> arguments) {
        try {
            return callee(@this, arguments);
        }
        finally {
            Unbind();
        }
    }

    public void Bind<T>(T? @this) {
        this.@this = @this;
    }

    public void Unbind() {
        this.@this = null;
    }

    public NativeFunction(int arity, Callee callee) {
        this.callee = callee;
        this.arity = arity;
    }
}