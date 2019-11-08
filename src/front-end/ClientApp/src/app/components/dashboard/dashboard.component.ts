import { Component, OnInit } from '@angular/core';
import { StickerService } from '../../services/sticker.service';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  isLoading = false;
  stickers: StickerResultModel[] = [];

  constructor(private _stickerService: StickerService) { }

  async ngOnInit(): Promise<void> {

    this.isLoading = true;
    this.stickers = [];
    try {
      this.stickers = await this._stickerService.getAll().toPromise();

      // TODO: remove. stub data.

      this.stickers = [{
        htmlColor: 'blue',
        id: 1,
        text: 'Preved',
        x: 50,
        y: 50,
      }, {
        htmlColor: 'red',
        id: 2,
        text: 'Preved 2',
        x: 100,
        y: 100,
      },
      ];



    } catch (error) {
      alert(error.error.message);
    } finally {
      this.isLoading = false;
    }
  }

  stickerTrackBy(index: number, item: StickerResultModel): number {
    return item.id;
  }
}
