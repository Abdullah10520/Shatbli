import { Component, output, signal, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { Ceramic } from '../../../../shared/models/design.model';
import { DesignService } from '../../../../Core/services/design.service';

@Component({
    selector: 'app-gallery-grid',
    standalone: true,
    imports: [CommonModule, TranslatePipe],
    templateUrl: './gallery-grid.component.html',
    styleUrl: './gallery-grid.component.css'
})
export class GalleryGridComponent implements OnInit {
    private designService = inject(DesignService);

    // المخرجات
    designSelected = output<Ceramic | null>();

    // الحالة
    items = signal<Ceramic[]>([]);
    selectedItem = signal<Ceramic | null>(null);
    isLoading = signal(false);

    ngOnInit(): void {
        this.loadCeramics();
    }

    private loadCeramics(): void {
        this.isLoading.set(true);
        this.designService.getAllCeramics().subscribe({
            next: () => {
                this.items.set(this.designService.ceramicsSig());
                this.isLoading.set(false);
            },
            error: () => {
                this.isLoading.set(false);
            }
        });
    }

    selectItem(item: Ceramic): void {
        // إذا تم النقر على نفس العنصر المختار، إلغاء الاختيار
        if (this.selectedItem()?.id === item.id) {
            this.selectedItem.set(null);
            this.designSelected.emit(null);
        } else {
            this.selectedItem.set(item);
            this.designSelected.emit(item);
        }
    }

    // إلغاء الاختيار من الخارج
    clearSelection(): void {
        this.selectedItem.set(null);
    }
}
