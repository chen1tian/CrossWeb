import './App.css'
import { useEffect, useState } from 'react'
import { notifyHostFrontendReady } from './host-bridge'

type AccessibleUrlsResponse = {
  urls?: string[]
}

function App() {
  const [pingState, setPingState] = useState<
    'idle' | 'loading' | 'success' | 'error'
  >('idle')
  const [pingMessage, setPingMessage] = useState('等待测试')
  const [accessibleUrls, setAccessibleUrls] = useState<string[]>([
    window.location.origin,
  ])

  useEffect(() => {
    let secondFrame = 0
    const firstFrame = window.requestAnimationFrame(() => {
      secondFrame = window.requestAnimationFrame(notifyHostFrontendReady)
    })

    return () => {
      window.cancelAnimationFrame(firstFrame)
      window.cancelAnimationFrame(secondFrame)
    }
  }, [])

  useEffect(() => {
    const abortController = new AbortController()

    void fetch('/api/host/urls', { signal: abortController.signal })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`地址接口返回 ${response.status}`)
        }
        return response.json() as Promise<AccessibleUrlsResponse>
      })
      .then((data) => {
        if (data.urls?.length) {
          setAccessibleUrls(data.urls)
        }
      })
      .catch((error: unknown) => {
        if (!(error instanceof DOMException && error.name === 'AbortError')) {
          console.warn('无法读取 CrossWebHost 可访问地址', error)
        }
      })

    return () => {
      abortController.abort()
    }
  }, [])

  async function sendPing() {
    setPingState('loading')
    setPingMessage('正在请求 CrossWebHost…')

    try {
      const response = await fetch('/api/ping')
      const body = await response.text()
      if (!response.ok || body.trim() !== 'pong') {
        throw new Error(`意外响应：${response.status} ${body}`)
      }

      setPingState('success')
      setPingMessage('pong')
    } catch (error) {
      setPingState('error')
      setPingMessage(error instanceof Error ? error.message : '请求失败')
    }
  }

  return (
    <main className="app-shell">
      <section className="hero" aria-labelledby="page-title">
        <p className="eyebrow">AVALONIA · REACT · PICOSERVER</p>
        <h1 id="page-title">CrossWeb 已连接</h1>
        <p className="intro">
          React 页面由 Avalonia CrossWebHost 内嵌的 PicoServer 同源提供。
        </p>

        <div className="connection-card">
          <div>
            <span className="label">CrossWebHost API</span>
            <strong
              className={`status status-${pingState}`}
              role="status"
              aria-live="polite"
            >
              <span className="status-dot" />
              {pingMessage}
            </strong>
          </div>
          <button
            type="button"
            onClick={sendPing}
            disabled={pingState === 'loading'}
          >
            {pingState === 'loading' ? '发送中…' : '发送 ping'}
          </button>
        </div>

        <section className="url-panel" aria-labelledby="accessible-url-title">
          <span className="label" id="accessible-url-title">
            可访问地址
          </span>
          <div className="url-list">
            {accessibleUrls.map((url) => (
              <code key={url}>{url}</code>
            ))}
          </div>
        </section>
      </section>
    </main>
  )
}

export default App
