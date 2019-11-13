import { Component, Input } from '@angular/core';
import { MenuItem } from 'src/app/models/view/menu-item';

@Component({
  selector: 'app-menu-item',
  templateUrl: './menu-item.component.html',
  styleUrls: ['./menu-item.component.css']
})
export class MenuItemComponent {
  @Input() item: MenuItem;
}
