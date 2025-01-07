import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react'
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const baseFolder =
    //path.join('C', 'Users', 'Krzys', 'AppData', 'Roaming', 'ASP.NET', 'https');
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "reactlibrary.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);


console.log(baseFolder)
if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}



if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status)
    {

        //throw new Error("Could not create certificate.");
    }
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7277';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
        react(),
    ],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/weatherforecast': {
                target,
                secure: false
            },
            '^/books': {
                target,
                secure: false
            },
            '^/book/[01]?[0-9][0-9]?|2[0-4][0-9]|25[0-5]': {
                target,
               secure: false,
               //changeOrigin: true,
               rewrite: path => {
                   const bookId = path.split('/')[2];
                   return `/books/${bookId}`;
                }
            },
            '^/user/register': {
                target,
                secure: false,
            },
            '^/user/login': {
                target,
                secure: false,
            },
            '^/user/logout': {
                target,
                secure: false,
            },
            '^/user/info': {
                target,
                secure: false,
                //rewrite: path => {return '/user/test'}
            }
        },
        port: 53747,
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        }
    }
})
