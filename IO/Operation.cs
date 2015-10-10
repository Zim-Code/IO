using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ZimCode.IO
{
    public class Operation
    {
        Action action;

        internal Operation(Type paramType = null, Type returnType = null)
        {
            ParamType = paramType;
            ReturnType = returnType;
        }

        private Operation(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            this.action = action;
        }

        public Type ParamType { get; private set; } 

        public Type ReturnType { get; private set; }

        internal virtual object ExecuteCore(object param)
        {
            action.Invoke();
            return null;
        }

        internal Task<object> ExecuteAsync(object param)
        {
            return Task.Factory.StartNew(() =>
                {
                    CheckParamType(param?.GetType());
                    return ExecuteCore(param);
                });
        }

        private void CheckParamType(Type paramType)
        {
            if (ParamType == null)
                return;
            if (!ParamType.GetTypeInfo().IsAssignableFrom(paramType.GetTypeInfo()))
                throw new ArgumentException(
                    $"The provided argument of type {paramType.FullName} " + 
                    $"does not match the expected param type of {ParamType.FullName}.");
        }

        internal bool IsValidReturnValue(Type returnType)
        {
            if (ReturnType == null)
                return true;
            return returnType.GetTypeInfo().IsAssignableFrom(ReturnType.GetTypeInfo());
        }

        public static Operation Do(Action action)
        {
            return new Operation(action);
        }

        public static Operation Consume<TParam>(Action<TParam> action)
        {
            return new ConsumeOperation<TParam>(action);
        }

        public static Operation Generate<TParam, TResult>(Func<TParam, TResult> func)
        {
            return new GenerateOperation<TParam, TResult>(func);
        }

        public static Operation Generate<TResult>(Func<TResult> func)
        {
            return new GenerateOperation<object, TResult>((o) => func());
        }
    }

    class ConsumeOperation<TParam> : Operation
    {
        Action<TParam> action;

        internal ConsumeOperation(Action<TParam> action)
            : base(typeof(TParam))
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            this.action = action;
        }

        internal override object ExecuteCore(object param)
        {
            action((TParam)param);
            return null;
        }
    }

    class GenerateOperation<TParam, TResult> : Operation
    {
        Func<TParam, TResult> func;

        internal GenerateOperation(Func<TParam, TResult> func)
        {
            this.func = func;
        }

        internal override object ExecuteCore(object param)
        {
            return func.Invoke((TParam)param);
        }
    }
}

