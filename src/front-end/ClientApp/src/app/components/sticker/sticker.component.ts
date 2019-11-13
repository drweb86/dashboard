import { Component, Input } from '@angular/core';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';

@Component({
  selector: 'app-sticker',
  templateUrl: './sticker.component.html',
  styleUrls: ['./sticker.component.css']
})
export class StickerComponent {
  @Input() item: StickerResultModel;
  @Input() isSelected: boolean;
}
