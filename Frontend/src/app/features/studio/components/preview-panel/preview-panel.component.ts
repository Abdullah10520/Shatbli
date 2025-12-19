import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GeneratedDesign } from '../../../../shared/models/design.model';

@Component({
    selector: 'app-preview-panel',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './preview-panel.component.html',
    styleUrl: './preview-panel.component.css'
})
export class PreviewPanelComponent {
    // المدخلات
    roomImageUrl = input<string | null>(null);
    generatedResult = input<GeneratedDesign | null>(null);
    isGenerating = input<boolean>(false);

    // المخرجات
    downloadClicked = output<void>();
    shareClicked = output<void>();

    // الحالة
    viewMode = signal<'original' | 'compare' | 'new'>('compare');
    sliderPosition = signal(50);

    setViewMode(mode: 'original' | 'compare' | 'new'): void {
        this.viewMode.set(mode);
    }

    onSliderMove(event: MouseEvent): void {
        const target = event.currentTarget as HTMLElement;
        const rect = target.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const percentage = Math.min(100, Math.max(0, (x / rect.width) * 100));
        this.sliderPosition.set(percentage);
    }

    download(): void {
        this.downloadClicked.emit();
    }

    share(): void {
        this.shareClicked.emit();
    }
}
