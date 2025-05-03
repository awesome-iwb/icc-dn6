using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DubiousDubiUniverse.InkCanvasForClass.Helpers
{
    /// <summary>
    /// Thread-safe async helper for reading/writing configuration files with optional backup, caching, and directory creation.
    /// </summary>
    public class ConfigurationFileHelper : IDisposable {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private volatile string _cache = string.Empty;
        private bool _disposed = false;

        /// <summary>
        /// 初始化 ConfigurationFileHelper 实例，指定要操作的配置文件路径。
        /// </summary>
        /// <param name="filePath">配置文件完整路径。</param>
        /// <exception cref="ArgumentNullException">当 filePath 为空或 null 时抛出。</exception>
        public ConfigurationFileHelper(string filePath) {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空。");
            _filePath = filePath;
        }

        /// <summary>
        /// 异步读取配置文件内容。如果文件不存在或读取失败，则返回缓存内容。
        /// </summary>
        public async Task<string> ReadAsync(CancellationToken cancellationToken = default) {
            ThrowIfDisposed();
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try {
                if (!File.Exists(_filePath)) {
                    _cache = string.Empty;
                    return _cache;
                }

                string content = await File.ReadAllTextAsync(_filePath, cancellationToken).ConfigureAwait(false);
                _cache = content;
                return content;
            }
            catch (FileNotFoundException) {
                _cache = string.Empty;
                return _cache;
            }
            catch (UnauthorizedAccessException) {
                throw;
            }
            catch (PathTooLongException) {
                throw;
            }
            catch (IOException) {
                return _cache;
            }
            finally {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 异步写入配置文件内容，写入前可选地创建 .bak 备份文件。
        /// </summary>
        /// <param name="content">要写入的新内容。</param>
        /// <param name="enableBackup">是否在写入前备份原文件，默认 true。</param>
        public async Task<bool> WriteAsync(string content, bool enableBackup = true,
            CancellationToken cancellationToken = default) {
            ThrowIfDisposed();
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            string backupPath = _filePath + ".bak";
            bool result = false;
            try {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);

                if (enableBackup && File.Exists(_filePath))
                    File.Copy(_filePath, backupPath, true);

                using (var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096,
                           FileOptions.WriteThrough))
                using (var writer = new StreamWriter(fs)) {
                    await writer.WriteAsync(content).ConfigureAwait(false);
                    await writer.FlushAsync().ConfigureAwait(false);
                    fs.Flush(true);
                }

                _cache = content;
                result = true;
            }
            catch (UnauthorizedAccessException) {
                throw;
            }
            catch (PathTooLongException) {
                throw;
            }
            catch (IOException) {
                if (enableBackup) {
                    try {
                        if (File.Exists(backupPath))
                            File.Copy(backupPath, _filePath, true);
                    }
                    catch {
                        /* 忽略恢复异常 */
                    }
                }

                result = false;
            }
            finally {
                _semaphore.Release();
            }

            return result;
        }

        /// <summary>
        /// 在同一锁上下文中执行多个操作，保证原子性。
        /// </summary>
        public async Task<T> ExecuteWithLockAsync<T>(Func<Task<T>> action,
            CancellationToken cancellationToken = default) {
            ThrowIfDisposed();
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try {
                return await action().ConfigureAwait(false);
            }
            finally {
                _semaphore.Release();
            }
        }

        private void ThrowIfDisposed() {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ConfigurationFileHelper));
        }

        /// <summary>
        /// 释放底层 SemaphoreSlim。
        /// </summary>
        public void Dispose() {
            if (!_disposed) {
                _semaphore.Dispose();
                _disposed = true;
            }
        }
    }
}
