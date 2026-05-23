import http from 'k6/http';
import { check } from 'k6';
import { Rate, Trend } from 'k6/metrics';

const errorRate = new Rate('errors');
const postDuration = new Trend('post_duration');
const getDuration = new Trend('get_duration');

const BASE_URL = 'http://localhost:5000';

export const options = {
  scenarios: {
    // 5000 peticiones POST por minuto (~83/s)
    post_orders: {
      executor: 'constant-arrival-rate',
      rate: 5000,
      timeUnit: '1m',
      duration: '1m',
      preAllocatedVUs: 100,
      maxVUs: 300,
      exec: 'postOrder',
    },
    // 5000 peticiones GET por minuto (~83/s)
    get_orders: {
      executor: 'constant-arrival-rate',
      rate: 5000,
      timeUnit: '1m',
      duration: '1m',
      preAllocatedVUs: 100,
      maxVUs: 300,
      exec: 'getOrders',
    },
  },
  thresholds: {
    // La prueba PASA si:
    'http_req_failed': ['rate<0.01'],          // menos del 1% de errores
    'http_req_duration': ['p(95)<2000'],        // 95% de requests bajo 2 segundos
    'post_duration': ['p(95)<2000'],
    'get_duration': ['p(95)<500'],
  },
};

export function postOrder() {
  const payload = JSON.stringify({
    customerId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
    total: Math.round(Math.random() * 1000 * 100) / 100,
  });

  const res = http.post(`${BASE_URL}/api/orders`, payload, {
    headers: { 'Content-Type': 'application/json' },
  });

  postDuration.add(res.timings.duration);

  const ok = check(res, {
    'POST → 202 Accepted': (r) => r.status === 202,
    'POST < 2s':           (r) => r.timings.duration < 2000,
  });

  errorRate.add(!ok);
}

export function getOrders() {
  const page = Math.floor(Math.random() * 10) + 1;
  const res = http.get(`${BASE_URL}/api/orders?page=${page}&pageSize=50`);

  getDuration.add(res.timings.duration);

  const ok = check(res, {
    'GET → 200 OK': (r) => r.status === 200,
    'GET < 500ms':  (r) => r.timings.duration < 500,
  });

  errorRate.add(!ok);
}
