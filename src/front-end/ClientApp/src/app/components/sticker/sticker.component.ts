import { Component, Input, Output, EventEmitter } from '@angular/core';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';

@Component({
  selector: 'app-sticker',
  templateUrl: './sticker.component.html',
  styleUrls: ['./sticker.component.css']
})
export class StickerComponent {
  @Input() item: StickerResultModel;
  @Input() isSelected: boolean;

  @Output() edit: EventEmitter<void> = new EventEmitter();
  @Output() delete: EventEmitter<void> = new EventEmitter();
  @Output() changeColor: EventEmitter<void> = new EventEmitter();

  onEdit(): void {
    this.edit.emit();
  }

  onDelete(): void {
    this.delete.emit();
  }

  onChangeColor(): void {
    this.changeColor.emit();
  }

  onHelp(): void {
    alert('Click on Note and drag it with mouse to move it.');
  }
}
