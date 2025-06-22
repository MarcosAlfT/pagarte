/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ApiResponse } from '../models/ApiResponse';
import type { ApiResponseOfstring } from '../models/ApiResponseOfstring';
import type { LoginRequest } from '../models/LoginRequest';
import type { RegisterUserRequest } from '../models/RegisterUserRequest';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class AuthService {
    /**
     * @param requestBody
     * @returns ApiResponseOfstring OK
     * @throws ApiError
     */
    public static postApiAuthRegister(
        requestBody: RegisterUserRequest,
    ): CancelablePromise<ApiResponseOfstring> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Auth/register',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @param token
     * @returns ApiResponse OK
     * @throws ApiError
     */
    public static getApiAuthConfirmEmail(
        token?: string,
    ): CancelablePromise<ApiResponse> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Auth/confirm-email',
            query: {
                'token': token,
            },
            errors: {
                400: `Bad Request`,
            },
        });
    }
    /**
     * @param requestBody
     * @returns ApiResponseOfstring OK
     * @throws ApiError
     */
    public static postApiAuthLogin(
        requestBody: LoginRequest,
    ): CancelablePromise<ApiResponseOfstring> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Auth/login',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }
}
