import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { StickerResultModel } from "../models/stickers/sticker-result-model";
import { StickerAddInputModel } from "../models/stickers/sticker-add-input-model";
import { StickerUpdateInputModel } from "../models/stickers/sticker-update-input-model";

@Injectable()
export class StickerService {
    constructor(private _http: HttpClient, @Inject('BASE_URL') private _baseUrl: string) {
    }

    add(info: StickerAddInputModel): Observable<void> {
        return this._http.post<void>(`${this._baseUrl}sticker`, info);
    }

    update(info: StickerUpdateInputModel): Observable<void> {
        return this._http.put<void>(`${this._baseUrl}sticker`, info);
    }

    delete(id: number): Observable<void> {
        return this._http.delete<void>(`${this._baseUrl}sticker/${id}`);
    }

    getAll(): Observable<StickerResultModel[]> {
        return this._http.get<StickerResultModel[]>(`${this._baseUrl}sticker`);
    }
}