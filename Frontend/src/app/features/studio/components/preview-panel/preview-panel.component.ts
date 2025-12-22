import { Component, input, output, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { GeneratedDesign } from '../../../../shared/models/design.model';
import { LanguageService } from '../../../../Core/services/language.service';

@Component({
    selector: 'app-preview-panel',
    standalone: true,
    imports: [CommonModule, TranslatePipe],
    templateUrl: './preview-panel.component.html',
    styleUrl: './preview-panel.component.css'
})
export class PreviewPanelComponent {
    readonly languageService = inject(LanguageService);

    // المدخلات
    roomImageUrl = input<string | null>(null);
    generatedResult = input<GeneratedDesign | null>(null);
    isGenerating = input<boolean>(false);
    progressMessage = input<string>('');

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
