import http from 'k6/http';
import { check } from 'k6';
import { uuidv4 } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

export const options = {
    stages: [
        { duration: '10s', target: 200 },
        { duration: '20s', target: 400 },
        { duration: '30s', target: 600 },
        { duration: '10s', target: 0 },
    ],
};


export default function () {
    const correlationId = uuidv4();
    const amount = 1;
    const requestedAt = new Date().toISOString();

    const payload = JSON.stringify({
        correlationId,
        amount,
        requestedAt,
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };    
    const res = http.post('http://localhost:9999/payment', payload, params);    
    check(res, {
        'status is 200': (r) => r.status === 200
    });
}

