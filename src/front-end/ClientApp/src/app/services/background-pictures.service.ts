import { BackgroundPicture } from '../models/view/background-picture';
import { Injectable } from '@angular/core';

@Injectable()
export class BackgroundPicturesService {
    async getBackgrounds(): Promise<BackgroundPicture[]> {
        const response = await fetch('assets/backgrounds/backgrounds.json');
        return await response.json();
    }
}