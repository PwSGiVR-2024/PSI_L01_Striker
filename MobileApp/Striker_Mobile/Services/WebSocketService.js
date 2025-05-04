class WebSocketService {
    constructor() {
      this.ws = null;
      this.serverUrl = null;
      this.reconnectInterval = 5000;
      this.shouldReconnect = false;
      this.listeners = {};
    }
  
    setServer(ip, port = 7373) {
      this.serverUrl = `ws://${ip}:${port}`;
    }
  
    connect() {
      if (!this.serverUrl) throw new Error('Server URL not set');
      this.shouldReconnect = true;
      this._createWebSocket();
    }
  
    _createWebSocket() {
      this.ws = new WebSocket(this.serverUrl);
  
      this.ws.onopen = () => this.emit('open');
      this.ws.onmessage = (e) => this.emit('message', e.data);
      this.ws.onerror = (e) => this.emit('error', e.message || 'WebSocket error');
      this.ws.onclose = () => {
        this.emit('close');
        this.ws = null;
        if (this.shouldReconnect) {
          setTimeout(() => this._createWebSocket(), this.reconnectInterval);
        }
      };
    }
  
    send(data) {
      if (this.ws && this.ws.readyState === WebSocket.OPEN) {
        const payload = typeof data === 'string' ? data : JSON.stringify(data);
        this.ws.send(payload);
      }
    }
  
    disconnect() {
      this.shouldReconnect = false;
      if (this.ws) {
        this.ws.close();
        this.ws = null;
      }
    }
  
    on(event, callback) {
      if (!this.listeners[event]) this.listeners[event] = new Set();
      this.listeners[event].add(callback);
    }
  
    off(event, callback) {
      this.listeners[event]?.delete(callback);
    }
  
    emit(event, data) {
      this.listeners[event]?.forEach(cb => cb(data));
    }
  
    removeAllListeners() {
      this.listeners = {};
    }
  }
  
  export default new WebSocketService();