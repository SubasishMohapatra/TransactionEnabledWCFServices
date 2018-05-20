using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Generischer Client für WCF Services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceClient<T>
    {
        private readonly ChannelFactory<T> ChannelFactory = new ChannelFactory<T>("*");

        /// <summary>
        /// Executes a wcf service call asynchronous.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>the result</returns>
        public async Task<TResult> ExecuteAsync<TResult>(Func<T, Task<TResult>> action)
        {
            IClientChannel clientChannel = (IClientChannel)ChannelFactory.CreateChannel();

            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            try
            {
                taskCompletionSource.TrySetResult(await DoExecuteAsync(action, clientChannel));
                clientChannel.Close();
            }   
            catch (Exception exception)
            {                
                taskCompletionSource.TrySetException(exception);
                clientChannel.Abort();
            }
            finally
            {
            }
            return await taskCompletionSource.Task;
        }

        private Task<TResult> DoExecuteAsync<TResult>(Func<T, Task<TResult>> action, IClientChannel clientChannel)
        {
            return action((T)clientChannel);
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>the result</returns>
        public TResult Execute<TResult>(Func<T, TResult> action)
        {
            IClientChannel clientChannel = (IClientChannel)ChannelFactory.CreateChannel();
            TResult result = default(TResult);
            try
            {
                result = action((T)clientChannel);
                clientChannel.Close();
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
                clientChannel.Abort();
            }
            catch (CommunicationException e)
            {
            }
            catch (Exception e)
            {
                clientChannel.Abort();
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Execute(Action<T> action)
        {
            IClientChannel clientChannel = (IClientChannel)ChannelFactory.CreateChannel();
            try
            {
                action((T)clientChannel);
                clientChannel.Close();
            }           
            catch (Exception exception)
            {
                clientChannel.Abort();
            }
            finally
            {
            }
        }
    }
}