type CrossWebHostWebViewBridge = {
  sendMessage: (message: unknown) => void
}

function getHostWebViewBridge(): CrossWebHostWebViewBridge | null {
  const avaloniaBridge = (
    window as unknown as {
      invokeCSharpAction?: (message: string) => void
    }
  ).invokeCSharpAction

  if (typeof avaloniaBridge === 'function') {
    return {
      sendMessage(message) {
        avaloniaBridge(
          typeof message === 'string' ? message : JSON.stringify(message),
        )
      },
    }
  }

  const candidate = (
    window as unknown as {
      chrome?: {
        webview?: {
          postMessage?: (message: unknown) => void
        }
      }
    }
  ).chrome?.webview

  if (typeof candidate?.postMessage === 'function') {
    return {
      sendMessage(message) {
        candidate.postMessage?.(message)
      },
    }
  }

  return null
}

export function notifyHostFrontendReady(): void {
  getHostWebViewBridge()?.sendMessage({ type: 'crossweb.frontendReady' })
}
